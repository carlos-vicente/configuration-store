var gulp = require('gulp');

// This task makes sure that when running gul
gulp.task('copyDependencies', function(){
    // scripts
    gulp.src('node_modules/jquery/dist/*.js').pipe(gulp.dest('Scripts/lib/jquery'));
    gulp.src('node_modules/materialize-css/dist/js/*.*').pipe(gulp.dest('Scripts/lib/materialize/'));
    gulp.src('node_modules/moment/min/*.js').pipe(gulp.dest('Scripts/lib/moment/'));
    gulp.src(['node_modules/react/umd/*.*', 'node_modules/react-dom/umd/*.*']).pipe(gulp.dest('Scripts/lib/react/'));
    gulp.src('node_modules/requirejs/require.js').pipe(gulp.dest('Scripts/lib/require'));
    gulp.src('node_modules/swagger-ui-dist/*.js*').pipe(gulp.dest('Scripts/lib/swagger'));
    gulp.src('node_modules/babel-polyfill/dist/*.js*').pipe(gulp.dest('Scripts/lib/babel-polyfill'));
    gulp.src('node_modules/whatwg-fetch/*.js*').pipe(gulp.dest('Scripts/lib/fetch'));
    gulp.src('node_modules/es6-promise/dist/*.js').pipe(gulp.dest('Scripts/lib/promise'));
    // gulp.src('node_modules/eventing-bus/lib/*.js').pipe(gulp.dest('Scripts/lib/eventing-bus'));
    gulp.src('node_modules/jsoneditor/dist/*.js').pipe(gulp.dest('Scripts/lib/jsoneditor'));

    // styles
    gulp.src('node_modules/materialize-css/dist/css/*.*').pipe(gulp.dest('Styles/materialize/css'));
    gulp.src('node_modules/materialize-css/dist/fonts/**/*.*').pipe(gulp.dest('Styles/materialize/fonts'));
    gulp.src('node_modules/swagger-ui-dist/*.png').pipe(gulp.dest('Styles/swagger/images'));
    gulp.src('node_modules/swagger-ui-themes/themes/3.x/*.*').pipe(gulp.dest('Styles/swagger/themes'));
    gulp.src('node_modules/jsoneditor/dist/*.css').pipe(gulp.dest('Styles/jsoneditor'));
    gulp.src('node_modules/jsoneditor/dist/img/*.*').pipe(gulp.dest('Styles/jsoneditor/img'));
});