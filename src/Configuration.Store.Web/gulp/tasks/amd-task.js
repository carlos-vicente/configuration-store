var gulp = require('gulp');
var requirejs = require('gulp-requirejs');


gulp.task('amd-bundle', function () {
    return requirejs({
        name: 'bootstrap',
        baseUrl: 'lib/app',
        out: 'app.js'
    }).pipe(gulp.dest("lib/app"));
});