var gulp = require('gulp');
var babel = require('gulp-babel');

var babelOptions = {
    presets: [
        'react', 
        ["env", { "targets": { "browsers": ["last 2 versions", "safari >= 7"] } }]],
    plugins: ['transform-es2015-modules-amd']
};

gulp.task('buildApp', function () {
    gulp.src('Scripts/Components/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('Scripts/app/'));

    // gulp.src('Scripts/Pages/*.jsx')
    //     .pipe(babel(babelOptions))
    //     .pipe(gulp.dest('Scripts/app/'));
});