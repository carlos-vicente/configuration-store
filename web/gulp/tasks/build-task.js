var gulp = require('gulp');
var babel = require('gulp-babel');

var babelOptions = {
    presets: [
        'react', 
        ["env", { "targets": { "browsers": ["last 4 versions", "safari >= 7"] } }]],
    plugins: ['transform-es2015-modules-amd']
};

gulp.task('buildApp', function () {
    gulp.src('scripts/modules/**/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('dist/app/modules/'));

     gulp.src('scripts/pages/*.jsx')
         .pipe(babel(babelOptions))
         .pipe(gulp.dest('dist/app/pages/'));
});