var gulp = require('gulp');
var webserver = require('gulp-webserver')
var minifyCSS = require('gulp-minify-css');
var config = {
    html: {
        watch: './app/**/*.hml'
    }
}

gulp.task('watch', function () {
    gulp.watch(config.html.watch)
});

gulp.task('server', function () {
    gulp.src('./')
        .pipe(webserver({
            host: '0.0.0.0',
            port: 8080,
            livereload: true
        }));
});

gulp.task('default', ['server']);