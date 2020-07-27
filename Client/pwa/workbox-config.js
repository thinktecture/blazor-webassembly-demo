module.exports = {
    maximumFileSizeToCacheInBytes: 10485760,
    globDirectory: "../bin/Debug/netstandard2.1/publish/wwwroot",
    globPatterns: [
        '**/*.{html,json,js,css,png,ico,json,wasm,dll,pdb,eot,otf,woff,svg,ttf}'
    ],
    swDest: "../bin/Debug/netstandard2.1/publish/wwwroot/sw.js",
    navigateFallback: "/index.html",
    clientsClaim: true,
    runtimeCaching: [{
        urlPattern: "https://localhost:5001/api/conferences/",
        handler: "NetworkFirst",
        options: {
            cacheName: "conferencesApi",
            expiration: {
                maxAgeSeconds: 1000,
            },
        },
    }],
};