var gulp = require('gulp');

gulp.task('copyMaterialize', function(){
    gulp.src('node_modules/materialize-css/dist/js/*.*')
        .pipe(gulp.dest('Scripts/lib/materialize/'));

    gulp.src('node_modules/materialize-css/dist/css/*.*')
        .pipe(gulp.dest('Styles/materialize/css'));

    gulp.src('node_modules/materialize-css/dist/fonts/**/*.*')
        .pipe(gulp.dest('Styles/materialize/fonts'));
});