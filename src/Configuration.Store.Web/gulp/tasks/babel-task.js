var gulp = require('gulp');
var babel = require('gulp-babel');

var babelOptions = {
    presets: ['es2017'],
    plugins: ['transform-react-jsx']
};

gulp.task('compileReactApp', function () {
    gulp.src('Components/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('lib/app/'));
});