module.exports = {
    globDirectory: "../bin/Debug/netstandard2.1/publish/BlazorConfTool.Client/dist",
    globPatterns: [
        '**/*.{html,json,js,css,png,ico,json,wasm,dll,pdb,eot,otf,woff,svg,ttf}'
    ],
    swDest: "../bin/Debug/netstandard2.1/publish/BlazorConfTool.Client/dist/sw.js",
    navigateFallback: "/index.html",
    clientsClaim: true,
    runtimeCaching: [{
        urlPattern: "https://localhost:44323/api/conferences/",
        handler: "NetworkFirst",
        options: {
            cacheName: "conferencesApi",
            expiration: {
                maxAgeSeconds: 1000,
            },
        },
    }],
};