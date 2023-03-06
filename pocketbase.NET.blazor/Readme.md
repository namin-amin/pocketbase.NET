> # Warning:Still under Development

# Pocketbase.NET

.NET C# SDK for [pocketbase](https://pocketbase.io/)

API's are Kept as similar as possible to [JS sdk](https://github.com/pocketbase/js-sdk) of pocketbase

```C#
using pocketbase.NET.blazor;

var Pb =  new  Pocketbase("http://127.0.0.1:8090");

var Records =  await Pb.Collections("posts").GetFullList();
```

## RoadMap

- [x] CRUD
- [x] Realtime
- [x] Basic Auth
- [ ] External Auth
- [ ] Test
