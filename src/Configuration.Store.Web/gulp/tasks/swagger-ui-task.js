var gulp = require('gulp');

// This task makes sure that when running gul
gulp.task('copySwaggerUiDist', function () {
    gulp.src('node_modules/swagger-ui-dist/*.js*')
        .pipe(gulp.dest('Scripts/lib/swagger'));

    gulp.src('node_modules/swagger-ui-dist/*.png')
        .pipe(gulp.dest('Styles/swagger/images'));

    gulp.src('node_modules/swagger-ui-themes/themes/3.x/*.*')
        .pipe(gulp.dest('Styles/swagger/themes'));
});