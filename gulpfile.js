var gulp = require("gulp");
var nuget = require('gulp-nuget');
var msbuild = require('gulp-msbuild');
var download = require("gulp-download");
var del = require('del');
var assemblyInfo = require('gulp-dotnet-assembly-info');
var xunit = xunit = require('gulp-xunit-runner');
var version = '1.5.2';

gulp.task('clean', ()=>{
    return del(['src/**/obj/', 'src/**/bin/Release', 'nuget.exe', 'nupkgs'])
});
gulp.task('downloadNuget', ['clean'], ()=>{
    return download('https://dist.nuget.org/win-x86-commandline/latest/nuget.exe')
    .pipe(gulp.dest('.'))
});

gulp.task('restore', ['downloadNuget'], ()=>{
    return gulp.src('./CompressR.sln')
    .pipe(nuget.restore({ nuget: "nuget.exe" }))
});
gulp.task('patchAssemblyInfo', ()=>{
    return gulp.src('**/AssemblyInfo.cs')
    .pipe(assemblyInfo({
        version: function(value) { return version; },
        fileVersion: function(value) { return version; },
        company: 'Tommy Parnell',
        copyright: 'Copyright © Tommy Parnell 2016'
    }))
    .pipe(gulp.dest('.'))
});
gulp.task('build', ['restore', 'patchAssemblyInfo'], ()=>{
     return gulp.src('./CompressR.sln', {read: false})
    .pipe(msbuild({
        stdout: true,
        toolsVersion: 14,
        configuration: 'Release'
    })); 
});
gulp.task('test', ['build'],  function () {
  return gulp.src(['src/*UnitTests/bin/Release/*UnitTests.dll'], {read: false})
    .pipe(xunit({
      executable: './packages/xunit.runner.console.2.1.0/tools/xunit.console.exe',
    }));
});
gulp.task('pack', ['test'], ()=>{
   return gulp.src(['src/CompressR.MVC4/*.csproj', 
   'src/CompressR.MVC5/*.csproj', 
   'src/CompressR.WebApi/*.csproj', 
   'src/CompressR/*.csproj', 
   'src/CompressR.Owin/*.csproj'], {read: false})
    .pipe(nuget.pack({
        build: false,
        symbols: true,
        properties: 'configuration=Release',
        outputDirectory: './nupkgs',
        includeReferencedProjects: true
    }));
});

gulp.task('publish', ['pack'], ()=>{
     return gulp.src(['!./nupkgs/*.symbols.nupkg','./nupkgs/*.nupkg'], {read: false})
    .pipe(nuget.push({ nuget: "nuget.exe", source: 'https://www.nuget.org/api/v2/package', apiKey: process.env.nugetApiKey}));
});