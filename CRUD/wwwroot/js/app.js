// wwwroot/js/app.js
const app = document.getElementById('app');
const apiBaseUrl = 'https://localhost:44350/api';

function renderLogin() {
    app.innerHTML = `
        <div class="container">
            <h2>Login</h2>
            <form id="loginForm">
                <div>
                    <label>Username:</label>
                    <input type="text" id="loginUsername" required>
                </div>
                <div>
                    <label>Password:</label>
                    <input type="password" id="loginPassword" required>
                </div>
                <button type="submit">Login</button>
            </form>
            <p>Don't have an account? <button class="nav-button" onclick="renderRegister()">Register</button></p>
        </div>
    `;

    document.getElementById('loginForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const username = document.getElementById('loginUsername').value;
        const password = document.getElementById('loginPassword').value;

        try {
            const response = await axios.post(`${apiBaseUrl}/auth/login`, { username, password });
            console.log('Login response:', response.data);
            localStorage.setItem('token', response.data.token);
            localStorage.setItem('username', username);
            console.log('Token saved:', localStorage.getItem('token'));
            renderProducts();
        } catch (error) {
            console.error('Login error:', error.response || error);
            alert('Invalid credentials');
        }
    });
}

function renderRegister() {
    app.innerHTML = `
        <div class="container">
            <h2>Register</h2>
            <form id="registerForm">
                <div>
                    <label>Username:</label>
                    <input type="text" id="registerUsername" required>
                </div>
                <div>
                    <label>Password:</label>
                    <input type="password" id="registerPassword" required>
                </div>
                <button type="submit">Register</button>
            </form>
            <p>Already have an account? <button class="nav-button" onclick="renderLogin()">Login</button></p>
        </div>
    `;

    document.getElementById('registerForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const username = document.getElementById('registerUsername').value;
        const password = document.getElementById('registerPassword').value;

        try {
            const response = await axios.post(`${apiBaseUrl}/auth/register`, { username, password });
            console.log('Register response:', response.data);
            localStorage.setItem('token', response.data.token);
            localStorage.setItem('username', username);
            console.log('Token saved:', localStorage.getItem('token'));
            renderProducts();
        } catch (error) {
            console.error('Register error:', error.response || error);
            alert('Registration failed: Username may already exist');
        }
    });
}

function renderProducts() {
    const token = localStorage.getItem('token');
    console.log('Token in renderProducts:', token);
    if (!token) {
        console.log('No token, redirecting to login');
        renderLogin();
        return;
    }

    app.innerHTML = `
        <div class="container">
            <h2>Products</h2>
            <p>Welcome, ${localStorage.getItem('username')}! <button class="nav-button" onclick="logout()">Logout</button></p>
            <ul id="productList"></ul>
            <div>
                <button id="prevBtn" disabled>Previous</button>
                <button id="nextBtn">Next</button>
            </div>
        </div>
    `;

    let page = 1;

    async function loadProducts() {
        try {
            const response = await axios.get(`${apiBaseUrl}/products?page=${page}&pageSize=10`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            console.log('Products response:', response.data);
            const products = response.data.items.$values; // Використовуємо response.data.items
            const productList = document.getElementById('productList');
            productList.innerHTML = products.map(p => `
                <li>
                    ${p.name} - $${p.price}
                    <button onclick="renderProductDetails(${p.id})">Details</button>
                </li>
            `).join('');
            document.getElementById('prevBtn').disabled = page === 1;
        } catch (error) {
            console.error('Products error:', error.response || error);
            logout();
        }
    }

    loadProducts();
    document.getElementById('nextBtn').addEventListener('click', () => { page++; loadProducts(); });
    document.getElementById('prevBtn').addEventListener('click', () => { page--; loadProducts(); });
}

function renderProductDetails(id) {
    const token = localStorage.getItem('token');
    if (!token) {
        renderLogin();
        return;
    }

    app.innerHTML = `
        <div class="container">
            <h2>Product Details</h2>
            <div id="productDetails"></div>
            <button onclick="renderProducts()">Back to Products</button>
        </div>
    `;

    axios.get(`${apiBaseUrl}/products/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
    })
        .then(response => {
            const p = response.data;
            document.getElementById('productDetails').innerHTML = `
                <p><strong>Name:</strong> ${p.name}</p>
                <p><strong>Price:</strong> $${p.price}</p>
                <p><strong>Category:</strong> ${p.category?.name || 'N/A'}</p>
                <h3>Details:</h3>
                <ul>${p.details.$values.map(d => `<li>${d.description}</li>`).join('')}</ul>
            `;
        })
        .catch(error => {
            console.error(error);
            logout();
        });
}

function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    renderLogin();
}

if (localStorage.getItem('token')) {
    renderProducts();
} else {
    renderLogin();
}