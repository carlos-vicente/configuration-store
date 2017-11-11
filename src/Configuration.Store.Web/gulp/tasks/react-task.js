var gulp = require('gulp');

gulp.task('copyReactDist', function(){
    gulp.src(['node_modules/react/umd/*.*',
        'node_modules/react-dom/umd/*.*'])
        .pipe(gulp.dest('Scripts/lib/react/'));
});