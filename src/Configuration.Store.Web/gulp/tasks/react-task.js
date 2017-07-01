var gulp = require('gulp');

gulp.task('copyReactDist', function(){
    gulp.src(['node_modules/react/dist/*.*',
        'node_modules/react-dom/dist/*.*'])
        .pipe(gulp.dest('lib/react/'));
});