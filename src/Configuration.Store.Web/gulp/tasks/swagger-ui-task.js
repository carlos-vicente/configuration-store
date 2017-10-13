var gulp = require('gulp');

// This task makes sure that when running gul
gulp.task('copySwaggerUiDist', function () {
    gulp.src('node_modules/swagger-ui-dist/*.*')
        .pipe(gulp.dest('swagger'));
});