var gulp = require('gulp');

gulp.task('copyMoment', function(){
    gulp.src('node_modules/moment/min/*.js')
        .pipe(gulp.dest('Scripts/lib/moment/'));
});