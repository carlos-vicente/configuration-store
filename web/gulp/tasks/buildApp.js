const gulp = require('gulp');
const babel = require('gulp-babel');

const babelOptions = {
    presets: [
        '@babel/preset-react',
        ["@babel/env", {"targets": {"browsers": ["last 4 versions", "safari >= 7"]}}]],
    plugins: ['@babel/plugin-transform-modules-amd']
};

function buildModules(cb){
    return gulp.src('scripts/modules/**/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('dist/app/modules/'))
        .on('end', cb);
}

function buildPages(cb){
    return gulp.src('scripts/pages/*.jsx')
        .pipe(babel(babelOptions))
        .pipe(gulp.dest('dist/app/pages/'))
        .on('end', cb);
}

gulp.task('buildModules', buildModules);
gulp.task('buildPages', buildPages);

gulp.task('buildApp', gulp.parallel('buildModules', 'buildPages'));