var gulp = require('gulp');

gulp.task('default', ['copyJquery', 'copyMaterialize', 'copyReactDist', 'copyRequireJs', 'build', 'amd-bundle'],
    function () {
    // this watch makes sure that when something changes in the watched files,
    // then task 'copyJquery' must be invoked
    //gulp.watch('bower_components/jquery/dist/*.js', ['copyJquery']);
    
    // just invoke all the others
});