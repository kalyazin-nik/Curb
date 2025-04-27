<template>
  <div>
    <h1>Погода на ближайшие дни</h1>
    <table>
      <tr>
        <th>Дата</th>
        <th>Температура</th>
        <th>Сводка</th>
      </tr>
      <tr v-for="forecast in forecasts" :key="forecast.date">
        <td>{{ forecast.date }}</td>
        <td>{{ forecast.temperatureC }}°C</td>
        <td>{{ forecast.summary }}</td>
      </tr>
    </table>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: 'WeatherForecast',
  data() {
    return {
      forecasts: []
    };
  },
  async mounted() {
    try {
      const token = localStorage.getItem("token");
      const response = await axios.get('/api/WeatherForecast', {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      });
      this.forecasts = response.data;
    } catch (error) {
      console.error('Ошибка при загрузке данных:', error);
    }
  }
}
</script>

<style scoped>
table {
  width: 100%;
  border-collapse: collapse;
  margin: 20px 0;
  font-size: 1rem;
  font-family: Arial, sans-serif;
  text-align: left;
}

th, td {
  padding: 10px 15px;
  border: 1px solid #494949;
}

th {
  background-color: #494949;
  font-weight: bold;
}

tr:hover {
  background-color: #5a695b;
}

h1 {
  color: #42b983;
  text-align: center;
  margin-bottom: 20px;
}
</style>
