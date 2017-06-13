var gulp = require('gulp');

gulp.task('copyReactWithRedux', function(){
    gulp.src(['node_modules/react/dist/*.*',
            'node_modules/redux/dist/*.*',
            'node_modules/react-redux/dist/*.*'])
        .pipe(gulp.dest('lib/react/'));
});