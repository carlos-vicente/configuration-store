var gulp = require('gulp');
var babel = require('gulp-babel');

//var browserify = require('browserify');
//var babelify = require('babelify');
//var source = require('vinyl-source-stream');
//var glob = require('glob');

var babelOptions = {
    presets: ['react', 'es2015'],
    plugins: ['transform-es2015-modules-amd']
};

gulp.task('build', function () {
    gulp.src('Components/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('lib/app/'));
    //var components = glob.sync('./Components/*.jsx');
    //return browserify({
    //        entries: components,
    //        extensions: ['.jsx'],
    //        debug: true
    //    })
    //    .transform(babelify, babelOptions)
    //    .bundle()
    //    .pipe(source('configStore.js'))
    //    .pipe(gulp.dest('lib/app/'));
});