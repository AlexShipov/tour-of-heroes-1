Tour of heroes https://angular.io/docs/ts/latest/tutorial/ using Webpack based on https://github.com/AngularClass/angular2-webpack-starter.git

to restore packages run : 

	npm install

for development run: 

	npm run start:hmr

scripts:
	
    runnet
    launches the dotnet (Kesterl) server on port 5001.
    
    build:prod
    cleans the dist folder and builds the angular project using the prod configuration.
    
    build:dev 
   	cleans the dist folder and builds the angular project using the dev configuration.
    
    clean:dist
    removes the dist compiled dll aot folders
    
    clean:zz
    removes the node_modules doc coverage dist compiled dll aot folders.
    
    lint
    runs tslint on the project
    
    server:dev:hmr
    start the dev server using hot module replacement.
    
    server:dev
    starts the dev server.
    
    server:prod
    starts the https server and serves files from the dist dir.
    
    start:hmr
    starts the dev server for the angular project using hot module replacement, and starts the .net server for the api calls.
    
    postpub
    copies the dist folder to the publish directory.
    
    dotnetpub
    runs dotnt publish command for the project using the release config.
    
    publish
    cleans the bin dir and runs all the publish steps.
