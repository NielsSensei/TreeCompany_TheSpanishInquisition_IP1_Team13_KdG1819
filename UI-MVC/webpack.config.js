const glob = require("glob");
// const PurgecssPlugin = require("purgecss-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        site: "./wwwroot/js/site.js",
        bootstrap_js: "./wwwroot/js/bootstrap_js.js",
        validation: "./wwwroot/js/validation.js",
        index: "./wwwroot/js/index.js",
        confirmemail: "./wwwroot/js/confirmemail.js",
        addQuestionnaireQuestion: "./wwwroot/js/addQuestionnaireQuestion.js"
    },
    output: {
        filename: "[name].entry.js",
        path: __dirname + "/wwwroot/dist"
    },
    devtool: "source-map",
    mode: "development",
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [{ loader: MiniCssExtractPlugin.loader }, "css-loader"]
            },
            //{ test: /\.css$/, loader: "style-loader!css-loader" },
            {
                test: /\.scss$/,
                use: [{
                    loader: "style-loader"
                }, {
                    loader: "css-loader"
                }, {
                    loader: "sass-loader"
                }]
            },
            { test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, loader: "file-loader" },
            { test: /\.(woff|woff2)$/, loader: "url-loader?prefix=font/&limit=5000" },
            {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
                loader: "url-loader?limit=10000&mimetype=application/octet-stream"
            },
            {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
                loader: "url-loader?limit=10000&mimetype=image/svg+xml"
            }]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css"
        })
        // Purgecss is left out
    ]
};
