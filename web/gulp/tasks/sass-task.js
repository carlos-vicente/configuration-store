var gulp = require('gulp');
var sass = require('gulp-sass');

gulp.task('buildCss', function () {
  return gulp.src('Styles/**/*.scss')
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest('Styles/app'));
});