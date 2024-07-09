export default {
  state() {
    return {
      applications: [
        {
          id: 'aBcDeFgHiJkLmNoPqRsTuVwXyZaBcDe',
          name: 'Application 1',
          date: '2022-07-15',
          server: 3,
          tasks: 1,
        },
        {
          id: 'BcDeFgHiJkLmNoPqRsTuVwXyZaBcDeFg',
          name: 'Application 2',
          date: '2021-11-23',
          server: 1,
          tasks: 3,
        },
        {
          id: 'CdEfGhIjKlMnOpQrStUvWxYzAbCdEfGh',
          name: 'Application 3',
          date: '2020-04-10',
          server: 4,
          tasks: 3,
        },
        {
          id: 'DeFgHiJkLmNoPqRsTuVwXyZaBcDeFgHi',
          name: 'Application 4',
          date: '2023-08-19',
          server: 2,
          tasks: 1,
        },
        {
          id: 'EfGhIjKlMnOpQrStUvWxYzAbCdEfGhIj',
          name: 'Application 5',
          date: '2021-01-05',
          server: 5,
          tasks: 6,
        },
        {
          id: 'FgHiJkLmNoPqRsTuVwXyZaBcDeFgHiJk',
          name: 'Application 6',
          date: '2020-06-15',
          server: 2,
          tasks: 0,
        },
        {
          id: 'GhIjKlMnOpQrStUvWxYzAbCdEfGhIjKl',
          name: 'Application 7',
          date: '2023-02-28',
          server: 1,
          tasks: 2,
        },
        {
          id: 'HiJkLmNoPqRsTuVwXyZaBcDeFgHiJkLm',
          name: 'Application 8',
          date: '2022-03-19',
          server: 4,
          tasks: 2,
        },
        {
          id: 'IjKlMnOpQrStUvWxYzAbCdEfGhIjKlMn',
          name: 'Application 9',
          date: '2021-10-20',
          server: 4,
          tasks: 1,
        },
        {
          id: 'JkLmNoPqRsTuVwXyZaBcDeFgHiJkLmNo',
          name: 'Application 10',
          date: '2020-09-12',
          server: 5,
          tasks: 3,
        },
        {
          id: 'KlMnOpQrStUvWxYzAbCdEfGhIjKlMnOp',
          name: 'Application 11',
          date: '2023-04-01',
          server: 2,
          tasks: 5,
        },
        {
          id: 'LmNoPqRsTuVwXyZaBcDeFgHiJkLmNoPq',
          name: 'Application 12',
          date: '2022-05-30',
          server: 2,
          tasks: 0,
        },
      ],
    };
  },
  getters: {
    getApps(state) {
      return state.applications;
    },
  },
};
