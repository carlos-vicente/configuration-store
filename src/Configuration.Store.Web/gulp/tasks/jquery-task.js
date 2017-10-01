var gulp = require('gulp');

// This task makes sure that when running gul
gulp.task('copyJquery', function(){
    gulp.src('node_modules/jquery/dist/*.js')
        .pipe(gulp.dest('Scripts/lib/jquery'));
});