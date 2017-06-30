var gulp = require('gulp');

gulp.task('copyReactWithRedux', function(){
    gulp.src(['node_modules/react/dist/*.*'])
        .pipe(gulp.dest('lib/react/'));
});