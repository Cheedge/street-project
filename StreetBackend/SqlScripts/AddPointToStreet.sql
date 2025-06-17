CREATE OR REPLACE FUNCTION add_point_to_street(
    in_street_id UUID,
    in_x DOUBLE PRECISION,
    in_y DOUBLE PRECISION,
    in_at_start BOOLEAN,
    in_old_version BYTEA,
    in_row_version BYTEA
)
RETURNS VOID AS $$
DECLARE
    current_geom GEOMETRY;
    new_point GEOMETRY := ST_SetSRID(ST_MakePoint(in_x, in_y), 4326);
    update_count INTEGER;
BEGIN
    -- Fetch current geometry
    SELECT ST_GeomFromText("GeometryWkt", 4326)
    INTO current_geom
    FROM "Streets"
    WHERE "Id" = in_street_id;

    -- Add new point at start or end
    IF in_at_start THEN
        current_geom := ST_MakeLine(new_point, current_geom);
    ELSE
        current_geom := ST_MakeLine(current_geom, new_point);
    END IF;

    -- Perform concurrency-safe update
    UPDATE "Streets"
    SET "GeometryWkt" = ST_AsText(current_geom),
        "RowVersion" = in_row_version
    WHERE "Id" = in_street_id
      AND "RowVersion" = in_old_version;

    GET DIAGNOSTICS update_count = ROW_COUNT;

    -- If no row was updated, version mismatch = concurrency conflict
    IF update_count = 0 THEN
        RAISE EXCEPTION 'Concurrency conflict: RowVersion mismatch for street ID %', in_street_id
            USING ERRCODE = '40001';  -- Serialization failure
    END IF;
END;
$$ LANGUAGE plpgsql;
