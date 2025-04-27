import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';

// Main App Component
const App = () => {
    return (
        <Router>
            <div className="bg-gray-100 min-h-screen">
                <nav className="bg-white shadow-sm">
                    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                        <div className="flex justify-between h-16">
                            <div className="flex">
                                <div className="flex-shrink-0 flex items-center">
                                    <Link to="/" className="text-xl font-bold text-orange-500">
                                        Food Delivery Admin
                                    </Link>
                                </div>
                                <div className="hidden sm:ml-6 sm:flex sm:space-x-8">
                                    <Link to="/restaurants" className="border-orange-500 text-gray-900 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                                        Restaurants
                                    </Link>
                                    <Link to="/orders" className="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                                        Orders
                                    </Link>
                                </div>
                            </div>
                        </div>
                    </div>
                </nav>

                <div className="py-10">
                    <div className="max-w-7xl mx-auto sm:px-6 lg:px-8">
                        <Routes>
                            <Route path="/" element={<Dashboard />} />
                            <Route path="/restaurants" element={<RestaurantManagement />} />
                            <Route path="/restaurants/:id/menu" element={<MenuManagement />} />
                            <Route path="/orders" element={<OrderManagement />} />
                        </Routes>
                    </div>
                </div>
            </div>
        </Router>
    );
};

// Dashboard Component
const Dashboard = () => {
    const [stats, setStats] = useState({
        restaurants: 0,
        orders: 0,
        revenue: 0
    });
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchStats = async () => {
            try {
                const [restaurantsRes, ordersRes] = await Promise.all([
                    fetch('/api/restaurant'),
                    fetch('/api/order')
                ]);

                const restaurants = await restaurantsRes.json();
                const orders = await ordersRes.json();

                const revenue = orders.reduce((sum, order) => sum + parseFloat(order.total), 0);

                setStats({
                    restaurants: restaurants.length,
                    orders: orders.length,
                    revenue: revenue
                });
                setLoading(false);
            } catch (error) {
                console.error('Error fetching stats:', error);
                setLoading(false);
            }
        };

        fetchStats();
    }, []);

    if (loading) {
        return (
            <div className="flex justify-center items-center h-64">
                <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-orange-500"></div>
            </div>
        );
    }

    return (
        <div>
            <h1 className="text-3xl font-bold text-gray-800 mb-6">Dashboard</h1>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
                <div className="bg-white rounded-lg shadow-md p-6">
                    <h2 className="text-lg font-semibold text-gray-700 mb-2">Restaurants</h2>
                    <p className="text-3xl font-bold text-orange-500">{stats.restaurants}</p>
                </div>
                <div className="bg-white rounded-lg shadow-md p-6">
                    <h2 className="text-lg font-semibold text-gray-700 mb-2">Total Orders</h2>
                    <p className="text-3xl font-bold text-orange-500">{stats.orders}</p>
                </div>
                <div className="bg-white rounded-lg shadow-md p-6">
                    <h2 className="text-lg font-semibold text-gray-700 mb-2">Total Revenue</h2>
                    <p className="text-3xl font-bold text-orange-500">${stats.revenue.toFixed(2)}</p>
                </div>
            </div>
        </div>
    );
};

// Restaurant Management Component
const RestaurantManagement = () => {
    const [restaurants, setRestaurants] = useState([]);
    const [loading, setLoading] = useState(true);
    const [formData, setFormData] = useState({
        name: '',
        address: '',
        phone: '',
        imageUrl: ''
    });

    useEffect(() => {
        fetchRestaurants();
    }, []);

    const fetchRestaurants = async () => {
        try {
            const response = await fetch('/api/restaurant');
            const data = await response.json();
            setRestaurants(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching restaurants:', error);
            setLoading(false);
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('/api/restaurant', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData)
            });

            if (response.ok) {
                fetchRestaurants();
                setFormData({
                    name: '',
                    address: '',
                    phone: '',
                    imageUrl: ''
                });
            }
        } catch (error) {
            console.error('Error adding restaurant:', error);
        }
    };

    const deleteRestaurant = async (id) => {
        try {
            const response = await fetch(`/api/restaurant/${id}`, {
                method: 'DELETE'
            });

            if (response.ok) {
                fetchRestaurants();
            }
        } catch (error) {
            console.error('Error deleting restaurant:', error);
        }
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center h-64">
                <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-orange-500"></div>
            </div>
        );
    }

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold text-gray-800 mb-6">Restaurant Management</h1>

            <div className="bg-white rounded-lg shadow-md p-6 mb-8">
                <h2 className="text-xl font-semibold text-gray-700 mb-4">Add New Restaurant</h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-gray-700 mb-2">Name</label>
                        <input
                            type="text"
                            name="name"
                            value={formData.name}
                            onChange={handleInputChange}
                            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 mb-2">Address</label>
                        <input
                            type="text"
                            name="address"
                            value={formData.address}
                            onChange={handleInputChange}
                            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 mb-2">Phone</label>
                        <input
                            type="text"
                            name="phone"
                            value={formData.phone}
                            onChange={handleInputChange}
                            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 mb-2">Image URL</label>
                        <input
                            type="text"
                            name="imageUrl"
                            value={formData.imageUrl}
                            onChange={handleInputChange}
                            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                        />
                    </div>
                    <button
                        type="submit"
                        className="bg-orange-500 text-white px-6 py-2 rounded-lg hover:bg-orange-600 transition duration-200"
                    >
                        Add Restaurant
                    </button>
                </form>
            </div>

            <div className="bg-white rounded-lg shadow-md p-6">
                <h2 className="text-xl font-semibold text-gray-700 mb-4">Restaurant List</h2>
                <div className="overflow-x-auto">
                    <table className="min-w-full bg-white">
                        <thead>
                            <tr>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Name</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Address</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Phone</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {restaurants.map(restaurant => (
                                <tr key={restaurant.id} className="hover:bg-gray-50">
                                    <td className="py-4 px-4 border-b border-gray-200">
                                        <Link to={`/restaurants/${restaurant.id}/menu`} className="text-orange-500 hover:text-orange-700">
                                            {restaurant.name}
                                        </Link>
                                    </td>
                                    <td className="py-4 px-4 border-b border-gray-200">{restaurant.address}</td>
                                    <td className="py-4 px-4 border-b border-gray-200">{restaurant.phone}</td>
                                    <td className="py-4 px-4 border-b border-gray-200 space-x-2">
                                        <Link
                                            to={`/restaurants/${restaurant.id}/menu`}
                                            className="text-blue-500 hover:text-blue-700"
                                        >
                                            Menu
                                        </Link>
                                        <button
                                            onClick={() => deleteRestaurant(restaurant.id)}
                                            className="text-red-500 hover:text-red-700"
                                        >
                                            Delete
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

// Menu Management Component
const MenuManagement = () => {
    const { id } = useParams();
    const [menuItems, setMenuItems] = useState([]);
    const [restaurant, setRestaurant] = useState(null);
    const [loading, setLoading] = useState(true);
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        price: '',
        imageUrl: ''
    });

    useEffect(() => {
        fetchRestaurantAndMenu();
    }, [id]);

    const fetchRestaurantAndMenu = async () => {
        try {
            const [restaurantRes, menuRes] = await Promise.all([
                fetch(`/api/restaurant/${id}`),
                fetch(`/api/restaurants/${id}/menu`)
            ]);

            const restaurantData = await restaurantRes.json();
            const menuData = await menuRes.json();

            setRestaurant(restaurantData);
            setMenuItems(menuData);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching data:', error);
            setLoading(false);
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const menuItem = {
                ...formData,
                restaurantId: parseInt(id),
                price: parseFloat(formData.price)
            };

            const response = await fetch(`/api/restaurants/${id}/menu`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(menuItem)
            });

            if (response.ok) {
                fetchRestaurantAndMenu();
                setFormData({
                    name: '',
                    description: '',
                    price: '',
                    imageUrl: ''
                });
            }
        } catch (error) {
            console.error('Error adding menu item:', error);
        }
    };

    const deleteMenuItem = async (menuItemId) => {
        try {
            const response = await fetch(`/api/restaurants/${id}/menu/${menuItemId}`, {
                method: 'DELETE'
            });

            if (response.ok) {
                fetchRestaurantAndMenu();
            }
        } catch (error) {
            console.error('Error deleting menu item:', error);
        }
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center h-64">
                <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-orange-500"></div>
            </div>
        );
    }

    if (!restaurant) {
        return <div className="text-center py-8">Restaurant not found</div>;
    }

    return (
        <div className="container mx-auto px-4 py-8">
            <div className="flex items-center mb-6">
                <Link to="/restaurants" className="text-orange-500 hover:text-orange-700 mr-4">
                    &larr; Back to Restaurants
                </Link>
                <h1 className="text-3xl font-bold text-gray-800">Menu for {restaurant.name}</h1>
            </div>

            <div className="bg-white rounded-lg shadow-md p-6 mb-8">
                <h2 className="text-xl font-semibold text-gray-700 mb-4">Add New Menu Item</h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-gray-700 mb-2">Name</label>
                        <input
                            type="text"
                            name="name"
                            value={formData.name}
                            onChange={handleInputChange}
                            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 mb-2">Description</label>
                        <textarea
                            name="description"
                            value={formData.description}
                            onChange={handleInputChange}
                            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                            rows="3"
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 mb-2">Price</label>
                        <input
                            type="number"
                            name="price"
                            value={formData.price}
                            onChange={handleInputChange}
                            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                            step="0.01"
                            min="0.01"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 mb-2">Image URL</label>
                        <input
                            type="text"
                            name="imageUrl"
                            value={formData.imageUrl}
                            onChange={handleInputChange}
                            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                        />
                    </div>
                    <button
                        type="submit"
                        className="bg-orange-500 text-white px-6 py-2 rounded-lg hover:bg-orange-600 transition duration-200"
                    >
                        Add Menu Item
                    </button>
                </form>
            </div>

            <div className="bg-white rounded-lg shadow-md p-6">
                <h2 className="text-xl font-semibold text-gray-700 mb-4">Menu Items</h2>
                <div className="overflow-x-auto">
                    <table className="min-w-full bg-white">
                        <thead>
                            <tr>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Name</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Description</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Price</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {menuItems.map(item => (
                                <tr key={item.id} className="hover:bg-gray-50">
                                    <td className="py-4 px-4 border-b border-gray-200">{item.name}</td>
                                    <td className="py-4 px-4 border-b border-gray-200">{item.description}</td>
                                    <td className="py-4 px-4 border-b border-gray-200">${item.price.toFixed(2)}</td>
                                    <td className="py-4 px-4 border-b border-gray-200">
                                        <button
                                            onClick={() => deleteMenuItem(item.id)}
                                            className="text-red-500 hover:text-red-700"
                                        >
                                            Delete
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

// Order Management Component
const OrderManagement = () => {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [statusFilter, setStatusFilter] = useState('all');

    useEffect(() => {
        fetchOrders();
    }, [statusFilter]);

    const fetchOrders = async () => {
        try {
            const url = statusFilter === 'all'
                ? '/api/order'
                : `/api/order?status=${statusFilter}`;

            const response = await fetch(url);
            const data = await response.json();
            setOrders(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching orders:', error);
            setLoading(false);
        }
    };

    const updateOrderStatus = async (orderId, status) => {
        try {
            const response = await fetch(`/api/order/${orderId}/status`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(status)
            });

            if (response.ok) {
                fetchOrders();
            }
        } catch (error) {
            console.error('Error updating order status:', error);
        }
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center h-64">
                <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-orange-500"></div>
            </div>
        );
    }

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold text-gray-800 mb-6">Order Management</h1>

            <div className="mb-6">
                <label className="block text-gray-700 mb-2">Filter by Status</label>
                <select
                    value={statusFilter}
                    onChange={(e) => setStatusFilter(e.target.value)}
                    className="px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500"
                >
                    <option value="all">All Orders</option>
                    <option value="Pending">Pending</option>
                    <option value="Preparing">Preparing</option>
                    <option value="Ready">Ready</option>
                    <option value="Delivered">Delivered</option>
                    <option value="Cancelled">Cancelled</option>
                </select>
            </div>

            <div className="bg-white rounded-lg shadow-md p-6">
                <div className="overflow-x-auto">
                    <table className="min-w-full bg-white">
                        <thead>
                            <tr>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Order ID</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Restaurant</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Items</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Total</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Status</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Date</th>
                                <th className="py-3 px-4 border-b border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {orders.map(order => (
                                <tr key={order.id} className="hover:bg-gray-50">
                                    <td className="py-4 px-4 border-b border-gray-200">#{order.id}</td>
                                    <td className="py-4 px-4 border-b border-gray-200">{order.restaurant?.name || 'N/A'}</td>
                                    <td className="py-4 px-4 border-b border-gray-200">
                                        <ul className="list-disc pl-5">
                                            {order.items.map(item => (
                                                <li key={item.id}>
                                                    {item.quantity}x {item.menuItem?.name || `Item ${item.menuItemId}`} (${item.price.toFixed(2)})
                                                </li>
                                            ))}
                                        </ul>
                                    </td>
                                    <td className="py-4 px-4 border-b border-gray-200">${order.total.toFixed(2)}</td>
                                    <td className="py-4 px-4 border-b border-gray-200">
                                        <span className={`px-2 py-1 rounded-full text-xs ${order.status === 'Pending' ? 'bg-yellow-100 text-yellow-800' :
                                                order.status === 'Preparing' ? 'bg-blue-100 text-blue-800' :
                                                    order.status === 'Ready' ? 'bg-green-100 text-green-800' :
                                                        order.status === 'Delivered' ? 'bg-purple-100 text-purple-800' :
                                                            'bg-red-100 text-red-800'
                                            }`}>
                                            {order.status}
                                        </span>
                                    </td>
                                    <td className="py-4 px-4 border-b border-gray-200">
                                        {new Date(order.createdAt).toLocaleString()}
                                    </td>
                                    <td className="py-4 px-4 border-b border-gray-200 space-x-2">
                                        {order.status !== 'Delivered' && order.status !== 'Cancelled' && (
                                            <select
                                                value={order.status}
                                                onChange={(e) => updateOrderStatus(order.id, e.target.value)}
                                                className="px-2 py-1 border rounded focus:outline-none focus:ring-1 focus:ring-orange-500 text-sm"
                                            >
                                                <option value="Pending">Pending</option>
                                                <option value="Preparing">Preparing</option>
                                                <option value="Ready">Ready</option>
                                                <option value="Delivered">Delivered</option>
                                                <option value="Cancelled">Cancelled</option>
                                            </select>
                                        )}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

// Render the app
ReactDOM.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>,
    document.getElementById('root')
);