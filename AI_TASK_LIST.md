 El siguiente objeto json es el error que recibo cuando no tengo autorizacion a una determinada accion de un feature:
---
{
"message": "Request failed with status code 403",
"name": "AxiosError",
"stack": "AxiosError: Request failed with status code 403\n    at settle (http://localhost:5173/node_modules/.vite/deps/axios.js?v=f174ec25:1257:12)\n    at XMLHttpRequest.onloadend (http://localhost:5173/node_modules/.vite/deps/axios.js?v=f174ec25:1606:7)\n    at Axios.request (http://localhost:5173/node_modules/.vite/deps/axios.js?v=f174ec25:2223:41)\n    at async Proxy.fetchAcademicYears (http://localhost:5173/src/stores/academicYearStore.ts:11:24)",
"config": {
"transitional": {
"silentJSONParsing": true,
"forcedJSONParsing": true,
"clarifyTimeoutError": false
},
"adapter": [
"xhr",
"http",
"fetch"
],
"transformRequest": [
null
],
"transformResponse": [
null
],
"timeout": 0,
"xsrfCookieName": "XSRF-TOKEN",
"xsrfHeaderName": "X-XSRF-TOKEN",
"maxContentLength": -1,
"maxBodyLength": -1,
"env": {},
"headers": {
"Accept": "application/json",
"Content-Type": "application/json",
"Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJmZTdmYjZkMS0yZWQzLTRhNTQtYmRhYy1kZmY4MTk2ZGYyZjAiLCJzdWIiOiJmZTU1MTZlMy0wYjIzLTQ4NTAtNjc1YS0wOGRlMzk4OTM3MWUiLCJlbWFpbCI6ImFkbWluQHNjaG9vbC5jb20iLCJnaXZlbl9uYW1lIjoiQWRtaW4iLCJmYW1pbHlfbmFtZSI6IlRlc3RVc2VyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NjU1OTI3ODMsImlzcyI6IkRlc3ljby5EbXMiLCJhdWQiOiJEZXN5Y28uRG1zIn0.DUPWFlBy8zz5oS3jE4dSrkU4VF9SSPG2cVefCTYo2wQ"
},
"baseURL": "/api/v1",
"withCredentials": true,
"method": "get",
"url": "/academic-years?$orderby=name asc&$top=10&$count=true",
"allowAbsoluteUrls": true
},
"code": "ERR_BAD_REQUEST",
"status": 403
}

--

Crea un plan para manejar este error que contemple lo slguiente:
- El backend sea capaz de transformar el error el formato problem detail que ya estamos utilzando en otras respuestas a errores.
- El frontend sea capaz de extraer el mensaje del error en formato problem detail y mostrarlo en un toast
- Crea un servicio de notificacion o composable que pueda ser reutilizado
