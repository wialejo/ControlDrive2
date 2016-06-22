var gulp = require('gulp');
var gutil = require('gulp-util');
var ftp = require('gulp-ftp');

gulp.task('deploy', function () {
    return gulp.src('src/*')
		.pipe(ftp({
		    host: 'ftp://69.64.43.33',
		    user: 'control',
		    pass: 'Loreka8812',
		    remotePath: '/ControlDriveApp_bdbkps'
		}))
		// you need to have some kind of stream after gulp-ftp to make sure it's flushed 
		// this can be a gulp plugin, gulp.dest, or any kind of stream 
		// here we use a passthrough stream 
		.pipe(gutil.noop());
});