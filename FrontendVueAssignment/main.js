Vue.createApp({
  mounted() {
    const savedExpenses = localStorage.getItem('expenses');
    if (savedExpenses) {
      this.expenses = JSON.parse(savedExpenses);
      if (this.expenses.length > 0) {
        this.nextId = this.expenses[this.expenses.length - 1].id + 1;
      } else {
        this.nextId = 1;
      }
      this.drawPieChart();
    } else {
      this.nextId = 1;
    }
  },
  data() {
    return {
      expenses: [],
      nextId: 1,
      expenseCategory: "",
      salary: null,
      dateToday: new Date().toISOString().substr(0, 10),
      objectName: "",
      selectedDay: null,
      sumString: "",
      date: new Date('2023-03-25'),
      expenseMonth: "20",
      expenseId: "",
      amount: Number(this.sumString),
      expanded: false,
      removeJakobsButton: false,
      categories: [
        'food',
        'transportation',
        'entertainment',
        'housing',
        'miscellaneous',
        'stocks'
      ],
    }
  },
  computed: {
    totalAmount() {
      let total = 0;
      this.expenses.forEach(expense => {
        if (expense.category === 'salary') {
          total += expense.amount;
        } else {
          total -= expense.amount;
        }
      });
      return total.toFixed(2);
    },

    remainingAmount() {
      if (this.salary === null) {
        return null;
      }
      return this.salary - this.totalAmount
    },
    filterExpenses() {
      const selectedMonth = this.expenseMonth;
      return this.expenses.filter(e => {
        const expenseDate = new Date(e.date);
        return expenseDate.getMonth() == selectedMonth;
      });
    },
    currentMonthExpenses() {
      const currentMonth = new Date().getMonth() + 1;
      return this.expenses.filter(expense => {
        const expenseMonth = new Date(expense.date).getMonth() + 1;
        return expenseMonth === currentMonth;
      });
    },
    currentMonthExpensesByCategory() {
      const expensesByCategory = {};
      this.currentMonthExpenses.filter(expense => expense.category !== 'salary').forEach(expense => {
        if (!expensesByCategory[expense.category]) {
          expensesByCategory[expense.category] = 0;
        }
        expensesByCategory[expense.category] += expense.amount;
      });

      return expensesByCategory;
    },

    currentMonthCategoryPercentages() {
      const filteredExpenses = this.currentMonthExpenses.filter(expense => expense.category !== 'salary');
      const totalExpenses = filteredExpenses.reduce((total, expense) => total + expense.amount, 0);
      const categoryPercentages = {};
      filteredExpenses.forEach(expense => {
        if (!categoryPercentages[expense.category]) {
          categoryPercentages[expense.category] = 0;
        }
        categoryPercentages[expense.category] += expense.amount;
      });
      Object.keys(categoryPercentages).forEach(category => {
        categoryPercentages[category] = (categoryPercentages[category] / totalExpenses) * 100;
      });
      return categoryPercentages;
    },
    //Metoden nedanför gör samma sak som de två över fast till vårana aside. Detta då vi inte lyckades lista ut hur vi skulle bearbeta parametrarna,
    //detta gör så att programmet blir en aning repetitivt men det fungerar som det ska.
    groupedExpenses() {
      const grouped = {};
      let totalAmount = 0;
      this.filterExpenses.forEach(expense => {
        if (expense.category === 'salary') {
          return;
        }
        totalAmount += expense.amount;
        if (!grouped[expense.category]) {
          grouped[expense.category] = {
            category: expense.category,
            amount: expense.amount
          };
        } else {
          grouped[expense.category].amount += expense.amount;
        }
      });
      Object.values(grouped).forEach(category => {
        category.percentage = Math.round(category.amount / totalAmount * 100);
      });
      return Object.values(grouped);
    }
  },
  methods: {
    addNumber(number) {
      this.sumString += number
    },
    removeNumber() {
      this.sumString = "";
    },
    handleButtonClick(month) {
      this.expandDiv();
      this.drawAsidePieChart(month);
    },
    addExpense() {
      if (!this.sumString || !this.expenseCategory) {
        return
      }
      const expense = {
        id: this.nextId++,
        amount: Number(this.sumString),
        category: this.expenseCategory,
        date: this.dateToday,
        name: this.objectName
      }
      this.sumString = "",
        this.objectName = "",
        this.expenseCategory = null
      if (this.salary !== null) {
        this.salary -= expense.amount
      }
      this.expenses.push(expense)
      this.drawPieChart();
      localStorage.setItem('expenses', JSON.stringify(this.expenses));
    },
    deleteTransaction(id) {
      const index = this.expenses.findIndex(expense => expense.id === id);
      if (index !== -1) {
        this.expenses.splice(index, 1);
        localStorage.setItem('expenses', JSON.stringify(this.expenses));
      }
      this.drawPieChart();

    },
    daysLeftToPayday(date) {
      const today = new Date();
      const lastDayOfMonth = new Date(
        today.getFullYear(),
        today.getMonth() + 1,
        0
      );
      const targetDay = date.getDate();
      const daysLeft = targetDay - today.getDate();
      return daysLeft > 0 ? daysLeft : daysLeft + lastDayOfMonth.getDate();
    },
    dailyAmount(date, sumString) {
      const today = new Date();
      const targetMonth = today.getMonth() <= 10 ? today.getMonth() + 2 : 1; // get next month, accounting for December
      const targetYear = targetMonth === 1 ? today.getFullYear() + 1 : today.getFullYear(); // get next year if next month is January
      const targetDate = new Date(`${targetYear}-${targetMonth}-25`); // set target date to the 25th of the next month
      const daysLeft = Math.ceil((targetDate - today) / (1000 * 60 * 60 * 24)); // calculate days left until next payday
      const amount = parseInt(sumString) / daysLeft;
      return amount.toFixed(2);
    },
    expandDiv() {
      this.expanded = true;
      this.toggleInActive();
      this.drawAsidePieChart();
    },
    closeDiv() {
      this.expanded = false;
      this.toggleInActive();
    },
    //Denna metoden används inte just nu, men vi valde att spara den för vi har en idé för framtiden.
    // toggleInActive() {
    //   this.isInActive = !this.isInActive;
    // },
    drawPieChart() {
      const canvas = this.$refs.canvas;
      const context = canvas.getContext('2d');
      const colors = {
        "food": "#FF0000",
        "transportation": "#FFFF00",
        "entertainment": "#008000",
        "housing": "#800080",
        "miscellaneous": "#008080",
        "stocks": "#808000"
      };

      context.clearRect(0, 0, canvas.width, canvas.height); // clear canvas

      const currentMonthExpenses = this.expenses.filter(expense => {
        const expenseDate = new Date(expense.date);
        return expenseDate.getMonth() === new Date().getMonth();
      });

      const data = this.categories.map(category => {
        return currentMonthExpenses.filter(expense => expense.category === category)
          .reduce((total, expense) => total + expense.amount, 0);
      });

      const total = data.reduce((a, b) => a + b, 0);
      //Här nedanför räknar vi ut mittpunkten på canvas elementet
      const centerX = canvas.width / 2;
      const centerY = canvas.height / 2;
      const radius = Math.min(canvas.width, canvas.height) / 2;
      let currentAngle = 0;

      //Nedanför så målar vi ut varje bit av cirkeldiagramet, vi påbörjar nästa bit där den förra slutade.
      data.forEach((value, index) => {
        const sliceAngle = (2 * Math.PI * value) / total;
        context.beginPath();
        context.moveTo(centerX, centerY);
        context.arc(centerX, centerY, radius, currentAngle, currentAngle + sliceAngle);
        context.fillStyle = colors[this.categories[index]];
        context.fill();
        currentAngle += sliceAngle;
      });
    },
    drawAsidePieChart(month) {
      const canvas = this.$refs.asideCanvas;
      const context = canvas.getContext('2d');
      const colors = {
        "food": "#FF0000",
        "transportation": "#FFFF00",
        "entertainment": "#008000",
        "housing": "#800080",
        "miscellaneous": "#008080",
        "stocks": "#808000"
      };

      context.clearRect(0, 0, canvas.width, canvas.height); // clear canvas

      const currentMonthExpenses = this.expenses.filter(expense => {
        const expenseDate = new Date(expense.date);
        return expenseDate.getMonth() === parseInt(month);
      });

      const data = this.categories.map(category => {
        return currentMonthExpenses.filter(expense => expense.category === category)
          .reduce((total, expense) => total + expense.amount, 0);
      });

      const total = data.reduce((a, b) => a + b, 0);

      const centerX = canvas.width / 2;
      const centerY = canvas.height / 2;
      const radius = Math.min(canvas.width, canvas.height) / 2;
      let currentAngle = 0;

      data.forEach((value, index) => {
        const sliceAngle = (2 * Math.PI * value) / total;
        context.beginPath();
        context.moveTo(centerX, centerY);
        context.arc(centerX, centerY, radius, currentAngle, currentAngle + sliceAngle);
        context.fillStyle = colors[this.categories[index]];
        context.fill();
        currentAngle += sliceAngle;
      });
    },
    jakobsKnapp() {
      const x1 = {
        id: 1,
        amount: 1000,
        category: "salary",
        date: this.dateToday,
        name: "swish"
      }
      const x2 = {
        id: 2,
        amount: 30000,
        category: "salary",
        date: this.dateToday,
        name: "lön"
      }
      const x3 = {
        id: 3,
        amount: 1500,
        category: "food",
        date: this.dateToday,
        name: "dadlar"
      }
      const x4 = {
        id: 4,
        amount: 3000,
        category: "food",
        date: this.dateToday,
        name: "mandelmjölk"
      }
      const x5 = {
        id: 5,
        amount: 100,
        category: "transportation",
        date: this.dateToday,
        name: "buss"
      }
      const x6 = {
        id: 6,
        amount: 25,
        category: "transportation",
        date: this.dateToday,
        name: "bolt"
      }
      const x7 = {
        id: 7,
        amount: 600,
        category: "entertainment",
        date: this.dateToday,
        name: "nintendo coins"
      }
      const x8 = {
        id: 8,
        amount: 200,
        category: "entertainment",
        date: this.dateToday,
        name: "Chatgpt premium"
      }
      const x9 = {
        id: 9,
        amount: 7000,
        category: "housing",
        date: this.dateToday,
        name: "rent"
      }
      const x10 = {
        id: 10,
        amount: 200,
        category: "housing",
        date: this.dateToday,
        name: "bath salt"
      }
      const x11 = {
        id: 11,
        amount: 400,
        category: "miscellaneous",
        date: this.dateToday,
        name: "Brad Pitt full scale model"
      }
      const x12 = {
        id: 12,
        amount: 300,
        category: "miscellaneous",
        date: this.dateToday,
        name: "Angelina Jolie full scale model"
      }
      const x13 = {
        id: 13,
        amount: 3000,
        category: "stocks",
        date: this.dateToday,
        name: "SKF"
      }
      const x14 = {
        id: 14,
        amount: 1500,
        category: "stocks",
        date: this.dateToday,
        name: "Ehang"
      }


      this.sumString = ""
      this.expenseCategory = null
      if (this.salary !== null) {
        this.salary -= expense.amount
      }
      this.expenses.push(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14)
      this.removeJakobsButton = true;
      this.drawPieChart();

    }
  },
}).mount('#app');