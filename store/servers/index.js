export default {
  state() {
    return {
      servers: [
        {
          id: 'aBcDeFgHiJkLmNoPqRsTuVwXyZaBcDe',
          name: 'Server 1',
          date: '2023-06-15',
          applications: 2,
          tasks: 7,
        },
        {
          id: 'BcDeFgHiJkLmNoPqRsTuVwXyZaBcDeFg',
          name: 'Server 2',
          date: '2022-11-23',
          applications: 4,
          tasks: 9,
        },
        {
          id: 'CdEfGhIjKlMnOpQrStUvWxYzAbCdEfGh',
          name: 'Server 3',
          date: '2021-04-10',
          applications: 1,
          tasks: 2,
        },
        {
          id: 'DeFgHiJkLmNoPqRsTuVwXyZaBcDeFgHi',
          name: 'Server 4',
          date: '2020-08-19',
          applications: 3,
          tasks: 8,
        },
        {
          id: 'EfGhIjKlMnOpQrStUvWxYzAbCdEfGhIj',
          name: 'Server 5',
          date: '2023-01-05',
          applications: 2,
          tasks: 4,
        },
      ],
    };
  },
  getters: {
    getServers(state) {
      return state.servers;
    },
  },
};
