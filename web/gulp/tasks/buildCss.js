const gulp = require('gulp');
const sass = require('gulp-sass');

function buildCss(cb) {
    return gulp.src('styles/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('dist/styles/app'))
        .on('end', cb);
}

gulp.task('buildCss', buildCss);