var gulp = require('gulp');
var babel = require('gulp-babel');

var babelOptions = {
    presets: ['react', 'es2015'],
    plugins: ['transform-es2015-modules-amd']
};

gulp.task('build', function () {
    gulp.src('Scripts/Components/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('Scripts/app/'));

    gulp.src('Scripts/Pages/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('Scripts/app/'));
});