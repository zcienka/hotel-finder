module.exports = {
    content: ["./src/**/*.{js,jsx,ts,tsx}",
        "node_modules/flowbite-react/**/*.{js,jsx,ts,tsx}",
        "./pages/**/*.{html,js}",
        "./components/**/*.{html,js}",
    ],
    theme: {
        fontFamily: {
            "roboto": ["Roboto", "sans-serif"],
        },
        extend: {
            spacing: {
                "128": "32rem",
                "160": "40rem",
                "192": "48rem",
                "224": "52rem",
                "256": "64rem",
            },
            colors: {
                "custom-blue-900": "#1F2229",
                "custom-blue-800": "#262A33",
                "custom-blue-700": "#2D313C",
            },
        }
    },
    plugins: ["tailwindcss, autoprefixer", "flowbite/plugin"],
}
