const path = require("path");
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

var babelOptions = {
    presets: [
        ["@babel/preset-env", {
            "targets": {
                "browsers": "last 2 Chrome versions, last 2 Firefox versions, last 2 Safari versions, last 2 iOS versions, last 2 Edge versions, last 2 Android versions"
                },
            "modules": false
        }],
        "@babel/preset-react"
    ],
    plugins: ["@babel/plugin-transform-runtime"]
};

module.exports = {
    devtool: "source-map",
    entry: './src/example.js',
    // entry: './src/CounterComponent.fs',
    // entry: './src/TodoComponent/TodoComponent.fs',
    mode: "development",
    output: {
        path: path.join(__dirname, './public'),
        filename: "bundle.js"
    },
    devServer: {
        contentBase: "./public",
        port: 8080,
    },
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        babel: babelOptions,
                        define: ["DEBUG"]
                    }
                }
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: babelOptions
                },
            },
            {
                test: /\.css$/i,
                use: [MiniCssExtractPlugin.loader, 'css-loader'],
            }
        ]
    },
    plugins: [new MiniCssExtractPlugin()]
    // ,resolve: {
    //     alias: {
    //         "react": "preact/compat",
    //         "react-dom/test-utils": "preact/test-utils",
    //         "react-dom": "preact/compat"
    //     }
    // }
};
