var gulp = require("gulp");
var nuget = require('gulp-nuget');
var msbuild = require('gulp-msbuild');
var download = require("gulp-download");
var del = require('del');
var assemblyInfo = require('gulp-dotnet-assembly-info');
var version = '1.0.0';

gulp.task('clean', ()=>{
    return del['./**/bin', './**/obj', 'nuget.exe', 'nupkgs']
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
    }))
    .pipe(gulp.dest('.'))
});
gulp.task('build', ['restore', 'patchAssemblyInfo'], ()=>{
     return gulp.src('./CompressR.sln')
    .pipe(msbuild({
        stdout: true,
        toolsVersion: 14,
        configuration: 'Release'
    })); 
});

gulp.task('pack', ['build'], ()=>{
   return gulp.src(['src/CompressR.MVC4/*.csproj', 'src/CompressR.MVC5/*.csproj', 'src/CompressR.WebApi/*.csproj'])
    .pipe(nuget.pack({
        build: false,
        properties: 'configuration=release',
        outputDirectory: './nupkgs'
    }));
});

gulp.task('publish', ['pack'], ()=>{
     return gulp.src('./nupkgs/*.nupkg')
    .pipe(nuget.push({ nuget: "nuget.exe", source: 'https://www.nuget.org/api/v2/package', apiKey: '158f98f2-f9e6-4490-9382-8b49ebda9cc7'}));
});