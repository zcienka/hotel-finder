import {fetchBaseQuery} from "@reduxjs/toolkit/dist/query/react";

const baseQuery = fetchBaseQuery({
        baseUrl: "http://localhost:8088/api/v1"
    }
)

export default baseQuery
