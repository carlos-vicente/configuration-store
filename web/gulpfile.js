/*
  gulpfile.js
  ===========
  Rather than manage one giant configuration file responsible
  for creating multiple tasks, each task has been broken out into
  its own file in gulp/tasks. Any files in that directory get
  automatically required below.
  To add a new task, simply add a new task file that directory.
  gulp/tasks/default.js specifies the default set of tasks to run
  when you run `gulp`.
*/

'use strict';

const gulp = require('gulp');
const HubRegistry = require('gulp-hub');

/* load some files into the registry */
const hub = new HubRegistry([
    'gulp/tasks/buildApp.js',
    'gulp/tasks/buildCss.js',
    'gulp/tasks/copyDependencies.js']);

/* tell gulp to use the tasks just loaded */
gulp.registry(hub);


gulp.task('default', gulp.series('copyDependencies', 'buildCss', 'buildApp'));