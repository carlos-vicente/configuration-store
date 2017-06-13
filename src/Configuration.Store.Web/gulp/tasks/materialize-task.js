var gulp = require('gulp');

gulp.task('copyMaterialize', function(){
    gulp.src('node_modules/materialize-css/dist/**/*.*')
        .pipe(gulp.dest('lib/materialize/'));
});