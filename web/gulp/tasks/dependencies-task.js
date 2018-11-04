var gulp = require('gulp');

// This task makes sure that when running gul
gulp.task('copyDependencies', function(){
    // scripts
    gulp.src('node_modules/jquery/dist/*.js').pipe(gulp.dest('dist/lib/jquery'));
    gulp.src('node_modules/materialize-css/dist/js/*.*').pipe(gulp.dest('dist/lib/materialize/'));
    gulp.src('node_modules/moment/min/*.js').pipe(gulp.dest('dist/lib/moment/'));
    gulp.src(['node_modules/react/umd/*.*', 'node_modules/react-dom/umd/*.*']).pipe(gulp.dest('dist/lib/react/'));
    gulp.src('node_modules/requirejs/require.js').pipe(gulp.dest('dist/lib/require'));
    gulp.src('node_modules/swagger-ui-dist/*.js*').pipe(gulp.dest('dist/lib/swagger'));
    gulp.src('node_modules/babel-polyfill/dist/*.js*').pipe(gulp.dest('dist/lib/babel-polyfill'));
    gulp.src('node_modules/whatwg-fetch/*.js*').pipe(gulp.dest('dist/lib/fetch'));
    gulp.src('node_modules/es6-promise/dist/*.js').pipe(gulp.dest('dist/lib/promise'));
    gulp.src('node_modules/jsoneditor/dist/*.js').pipe(gulp.dest('dist/lib/jsoneditor'));

    // styles
    gulp.src('node_modules/materialize-css/dist/css/*.*').pipe(gulp.dest('dist/styles/materialize/css'));
    gulp.src('node_modules/materialize-css/dist/fonts/**/*.*').pipe(gulp.dest('dist/styles/materialize/fonts'));
    gulp.src('node_modules/swagger-ui-dist/*.png').pipe(gulp.dest('dist/styles/swagger/images'));
    gulp.src('node_modules/swagger-ui-themes/themes/3.x/*.*').pipe(gulp.dest('dist/styles/swagger/themes'));
    gulp.src('node_modules/jsoneditor/dist/*.css').pipe(gulp.dest('dist/styles/jsoneditor'));
    gulp.src('node_modules/jsoneditor/dist/img/*.*').pipe(gulp.dest('dist/styles/jsoneditor/img'));
});