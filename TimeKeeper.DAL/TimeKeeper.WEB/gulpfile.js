var gulp = require('gulp');
var jshint = require('gulp-jshint');
var concat = require('gulp-concat');
var clean = require('gulp-clean-css');
var rename = require('gulp-rename');
var minify = require('gulp-minify');
var uglify = require('gulp-uglify');
var template = require('gulp-angular-templatecache');
var gutil = require('gulp-util');
var browserSync = require('browser-sync').create();
var browserify = require('browserify');
var merge = require('utils-merge');

var source = ["scripts/*.js", "scripts/**/*.js"];
var library = ['library/angular.min.js', 'library/angular-route.min.js', 'library/ui-bootstrap-tpls.min.js'];
var html = 'views/*.html';
var style = ['styles/**/*.css', 'styles/**/*.min.css','styles/*.min.css', 'styles/*.css', 'styles/**/*.min.css'];
var images= ['images/**/*.jpg']

gulp.task('jshint', function() {
    gulp.src(source)
        .pipe(jshint())
        .pipe(jshint.reporter('gulp-jshint-file-reporter', { filename: './jshint-output.log' }));
});

gulp.task('lib', function(){
    return gulp.src(library)
        .pipe(concat('lib.min.js'))
        .pipe(gulp.dest('dist'));
});

gulp.task('css', function(){
    return gulp.src(style)
        .pipe(concat('app.css'))
        .pipe(clean('app.css'))
        .pipe(rename('app.min.css'))
        .pipe(gulp.dest('dist'));
});

gulp.task('app', function() {
    return gulp.src(source)
        .pipe(concat('app.js'))
        .pipe(uglify())
        .pipe(rename('app.min.js'))
        .pipe(gulp.dest('dist'));
});

gulp.task('html', function () {
    return gulp
        .src(html)
        .pipe(template('template.js', { module: 'timeKeeper', root: 'views/' }))
        .pipe(gulp.dest('dist'));
});

gulp.task('images', function () {
    return gulp.src(images)
        .pipe(gulp.dest('dist/images'))
})
gulp.task('browse', ['lib', 'app', 'html', 'css' ,'images'], function(){
    browserSync.init({
        server: {
            baseDir: "dist"
        },
        browser: "chrome"
    });
});

gulp.task('default', function(){
    browserSync.init({
        server: {
            baseDir: "./",
            index: "index.html"
        },
        port: 3000,
        browser: "chrome"
    });
});