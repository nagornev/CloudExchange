const host = "http://localhost:7000";

export const API={
    host: host,
    endpoints:{
        file: `${host}/file`
    },

    getEndpoint(path){
        return `${host}${path}`
    }
}