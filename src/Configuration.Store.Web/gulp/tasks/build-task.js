var gulp = require('gulp');
var babel = require('gulp-babel');

var babelOptions = {
    presets: [
        'react', 
        ["env", { "targets": { "browsers": ["last 4 versions", "safari >= 7"] } }]],
    plugins: ['transform-es2015-modules-amd']
};

gulp.task('buildApp', function () {
    gulp.src('Scripts/Modules/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('Scripts/app/Modules/'));

     gulp.src('Scripts/Pages/*.jsx')
         .pipe(babel(babelOptions))
         .pipe(gulp.dest('Scripts/app/Pages/'));
});