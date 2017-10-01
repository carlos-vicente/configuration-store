var gulp = require('gulp');

// This task makes sure that when running gul
gulp.task('copyRequireJs', function () {
    gulp.src('node_modules/requirejs/require.js')
        .pipe(gulp.dest('Scripts/lib/require'));
});