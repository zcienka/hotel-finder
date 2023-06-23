/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./src/**/*.{js,jsx,ts,tsx}",
        'node_modules/flowbite-react/**/*.{js,jsx,ts,tsx}',
        './pages/**/*.{html,js}',
        './components/**/*.{html,js}',
    ],
    theme: {
        fontFamily: {
            'roboto': ['Roboto', 'sans-serif'],
        },
        extend: {
            spacing: {
                '128': '32rem',
                '160': '40rem',
            }
        }
    },
    plugins: ["tailwindcss, autoprefixer", "flowbite/plugin"],
}
