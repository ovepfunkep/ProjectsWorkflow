<template>
    <div class="text-center" style="align-self:start">
        <v-snackbar v-model="snackbar"
                    :snackbarTimeout="snackbarTimeout"
                    @click="snackbar = false"
                    :color="snackbarColor"
                    :top="true">
            {{ snackbarText }}
        </v-snackbar>
    </div>
    <div class="main">
        <v-btn style="position: sticky; top: 85%" v-if="loaded" size="x-large" color="blue" icon="mdi-plus" @click="addNewItem()"></v-btn>
        <div v-if="!loaded">
            <v-label style="margin-right:5px; ">Loading..</v-label>
            <v-progress-circular indeterminate></v-progress-circular>
        </div>

        <div style="width: 80%; margin: 0 auto;">
            <v-btn prepend-icon="mdi-account"
                   stacked
                   :variant="buttonEmployeesVariant"
                   @click="loadEmployees()"> Employees </v-btn>
            <v-btn prepend-icon="mdi-folder"
                   stacked
                   :variant="buttonProjectsVariant"
                   @click="loadProjects()"> Projects </v-btn>
            <v-text-field v-model="search"
                          label="Поиск"
                          single-line
                          hide-details
                          maxlength="100"
                          style="margin-top: 20px">
            </v-text-field>
            <v-data-table v-model:items-per-page="itemsPerPage"
                          v-model:page="page"
                          :headers="headers"
                          :items="tableData"
                          :search="search"
                          @click:row="rowClick"
                          single-select
                          :items-per-page="itemsPerPage"
                          class="elevation-6">
                <template v-slot:bottom>
                    <div class="text-center pt-2">
                        <v-pagination v-model="page"
                                      :length="pageCount"></v-pagination>
                    </div>
                </template>
                <template v-slot:item.projectManager="{ item }">
                    {{ getFullName(item.columns.projectManager) }}
                </template>
                <template v-slot:item.position="{ item }">
                    {{ item.columns.position?.name }}
                </template>
            </v-data-table>
        </div>
        <div class="placeForButton" style="height:50px"></div>

        <v-dialog v-model="showEditingDialog" max-width="500px">
            <v-card>
                <v-card-title>Edit Item</v-card-title>
                <v-card-text>
                    <template v-if="selectedItem">
                        <v-form ref="employeeForm" v-if="isEmployeesTable">
                            <v-row>
                                <v-col cols="12" sm="6">
                                    <v-text-field variant="outlined" v-model="selectedItem.surname" label="Surname" maxlength="100"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6">
                                    <v-text-field variant="outlined" v-model="selectedItem.name" label="Name" maxlength="100" :rules="[v => !!v || 'Name is required', v => (v && v.length <= 100) || 'Maximum 100 characters']"></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12" sm="6">
                                    <v-text-field variant="outlined" v-model="selectedItem.patronymic" label="Patronymic" maxlength="100"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" class="text-center">
                                    <v-combobox variant="outlined"
                                                v-model="selectedItem.position"
                                                :items="allPositions"
                                                label="Position"
                                                item-title="name"
                                                maxlength="100"
                                                :rules="[v => !!v || 'Position must be assigned']"
                                                @change="handlePositionChange"></v-combobox>
                                    <v-btn variant="text" v-if="selectedItem.position" color="light-gray" @click="changePosition">Update position</v-btn>
                                </v-col>
                            </v-row>
                        </v-form>
                        <v-form ref="projectForm" v-else-if="isProjectsTable">
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field variant="outlined" v-model="selectedItem.name" label="Name" maxlength="100" :rules="[v => !!v || 'Name is required']"></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field variant="outlined" v-model="selectedItem.customerCompany" label="Customer Company" maxlength="100" :rules="[v => !!v || 'Name is required']"></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field variant="outlined" v-model="selectedItem.executorCompany" label="Executor Company" maxlength="100" :rules="[v => !!v || 'Name is required']"></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12" sm="6">
                                    <v-text-field variant="outlined" type="date" v-model="selectedItem.startDate" label="Start Date"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6">
                                    <v-text-field variant="outlined" type="date" v-model="selectedItem.endDate" label="End Date"></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-slider v-model="selectedItem.priority" label="Priority" :min="1" :max="10" :step="1" thumb-label></v-slider>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-autocomplete variant="outlined"
                                                    v-model="selectedItem.projectManager"
                                                    :items="allEmployees"
                                                    label="Project Manager"
                                                    :item-title="getFullName"
                                                    return-object="true"></v-autocomplete>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-autocomplete variant="outlined"
                                                    v-model="selectedItem.employees"
                                                    :items="filteredEmployees"
                                                    label="Assigned employees"
                                                    :item-title="getFullName"
                                                    multiple
                                                    return-object></v-autocomplete>

                                </v-col>
                            </v-row>
                        </v-form>
                    </template>
                </v-card-text>
                <v-card-actions style="justify-content: space-between">
                    <v-col cols="12" sm="6">
                        <v-btn variant="elevated" color="#94ff94" @click="saveEditing">Save</v-btn>
                    </v-col>
                    <v-col cols="12" sm="6" class="text-end">
                        <div v-if="selectedItem?.id">
                            <v-btn variant="elevated" color="#ff9494" @click="deleteEditing">Delete</v-btn>
                        </div>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-dialog v-model="showPositionDialog" max-width="500px">
            <v-card>
                <v-card-title>Position</v-card-title>
                <v-card-text variant="outlined">
                    <v-form ref="positionForm">
                        <v-text-field v-model="selectedItemPositionName" label="Position Name" maxlength="100" :rules="[p => !!p  || 'Position required']"></v-text-field>
                    </v-form>
                </v-card-text>
                <v-card-actions style="justify-content: space-between">
                    <v-btn variant="elevated" color="blue" text @click="updatePosition">Update</v-btn>
                    <v-btn variant="elevated" color="#ff3636" text @click="deletePosition">Delete</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-dialog v-model="showDialogClose" max-width="400px">
            <v-card>
                <v-card-text>Are you sure you want to close? Any unsaved changes will be lost.</v-card-text>
                <v-card-actions style="justify-content: space-between">
                    <v-btn color="blue" text @click="openEditingAgain">Cancel</v-btn>
                    <v-btn color="red" text @click="showCloseCheck = false">Close</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>

</template>

<script>
    import { VDataTable } from 'vuetify/labs/VDataTable'
    import axios from 'axios'
    export default {
        name: "indexPage",
        components: {
            VDataTable
        },
        data() {
            return {
                loaded: true,
                selectedItem: null,
                showEditingDialog: false,
                showPositionDialog: false,
                showCloseCheck: false,
                search: '',
                headers: [],
                page: 1,
                itemsPerPage: 10,
                tableData: [],
                buttonEmployeesVariant: 'default',
                buttonProjectsVariant: 'default',
                buttonPositionsVariant: 'default',
                allEmployees: [],
                allProjects: [],
                allPositions: [],
                snackbar: false,
                snackbarText: '',
                snackbarColor: '',
                snackbarTimeout: 5000
            }
        },
        mounted() {
            this.loadProjects()
        },
        computed: {
            pageCount() {
                return Math.ceil(this.tableData.length / this.itemsPerPage)
            },

            isEmployeesTable() {
                return this.buttonEmployeesVariant === 'outlined';
            },

            isProjectsTable() {
                return this.buttonProjectsVariant === 'outlined';
            },

            selectedItemPositionName: {
                get() {
                    return this.selectedItem.position?.name;
                },
                set(value) {
                    this.selectedItem.position.name = value;
                }
            },

            showDialogClose: {
                get() {
                    return (!this.showEditingDialog && this.showCloseCheck);
                },
                set(value) {
                    this.showCloseCheck = value;
                    this.showEditingDialog = !value;
                }
            },

            filteredEmployees() {
                return this.allEmployees.filter(employee => employee.id !== this.selectedItem.projectManager?.id);
            }

        },
        methods: {
            rowClick: function (item, row) {
                this.selectedItem = JSON.parse(JSON.stringify(row.item.raw));
                this.showEditingDialog = true;
                this.showCloseCheck = true;
            },

            addNewItem() {
                let newItem = {};

                if (this.isEmployeesTable) {
                    newItem = {
                        surname: '',
                        name: '',
                        patronymic: '',
                        position: null,
                    };
                } else if (this.isProjectsTable) {
                    newItem = {
                        name: '',
                        customerCompany: '',
                        executorCompany: '',
                        startDate: null,
                        endDate: null,
                        priority: 1,
                        projectManager: null,
                        employees: [],
                    };
                }

                this.selectedItem = newItem;
                this.showEditingDialog = true;
                this.showCloseCheck = true;
            },

            loadEmployees() {
                this.loaded = false;

                this.headers = [
                    {
                        title: 'Id',
                        align: 'center',
                        key: 'id'
                    },
                    {
                        title: 'Фамилия',
                        align: 'center',
                        key: 'surname'
                    },
                    {
                        title: 'Имя',
                        align: 'center',
                        key: 'name',
                    },
                    {
                        title: 'Отчество',
                        align: 'center',
                        key: 'patronymic',
                    },
                    {
                        title: 'Должность',
                        align: 'center',
                        key: 'position',
                    }];

                axios.get('https://localhost:7129/api/Positions')
                    .then(response => {
                        this.allPositions = response.data;
                        this.loaded = true;
                    })
                    .catch(error => {
                        this.snackbarText = `Something went wrong on loading positions...\n${error}`;
                        this.snackbarColor = '#ff3636';
                        this.snackbar = true;
                        this.loaded = true;
                    });

                axios.get('https://localhost:7129/api/Employees')
                    .then(response => {
                        this.allEmployees = response.data;
                        this.tableData = this.allEmployees;
                        this.buttonProjectsVariant = 'default';
                        this.buttonEmployeesVariant = 'outlined';

                        this.loaded = true;
                    })
                    .catch(error => {
                        this.snackbarText = `Something went wrong on loading employees...\n${error}`;
                        this.snackbarColor = '#ff3636';
                        this.snackbar = true;
                        this.loaded = true;
                    });
            },

            loadProjects() {
                this.loaded = false;

                this.headers = [
                    {
                        title: 'Id',
                        align: 'center',
                        key: 'id'
                    },
                    {
                        title: 'Название',
                        align: 'center',
                        key: 'name'
                    },
                    {
                        title: 'Компания заказчика',
                        align: 'center',
                        key: 'customerCompany',
                    },
                    {
                        title: 'Компания исполнителя',
                        align: 'center',
                        key: 'executorCompany',
                    },
                    {
                        title: 'Дата начала',
                        align: 'center',
                        key: 'startDate',
                    },
                    {
                        title: 'Дата окончания',
                        align: 'center',
                        key: 'endDate',
                    },
                    {
                        title: 'Приоритет',
                        align: 'center',
                        key: 'priority',
                    },
                    {
                        title: 'Руководитель',
                        align: 'center',
                        key: 'projectManager'
                    },
                    {
                        title: 'Employees',
                        align: 'center',
                        key: 'employees',
                        value: 'employees.length',
                    }];

                axios.get('https://localhost:7129/api/Employees')
                    .then(response => {
                        this.allEmployees = response.data;
                        this.loaded = true;
                    })
                    .catch(error => {
                        this.snackbarText = `Something went wrong on loading employees...\n${error}`;
                        this.snackbarColor = '#ff3636';
                        this.snackbar = true;
                        this.loaded = true;
                    });

                axios.get('https://localhost:7129/api/Projects')
                    .then(response => {
                        this.allProjects = response.data
                        this.tableData = this.allProjects;
                        this.buttonProjectsVariant = 'outlined';
                        this.buttonEmployeesVariant = 'default';
                        this.loaded = true;
                    })
                    .catch(error => {
                        this.snackbarText = `Something went wrong on loading projects...\n${error}`;
                        this.snackbarColor = '#ff3636';
                        this.snackbar = true;
                        this.loaded = true;
                    });
            },

            loadPositions() {
                this.loaded = false;

                axios.get('https://localhost:7129/api/Positions')
                    .then(response => {
                        this.allPositions = response.data;
                    })
                    .catch(error => {
                        this.snackbarText = `Something went wrong on loading positions...\n${error}`;
                        this.snackbarColor = '#ff3636';
                        this.snackbar = true;
                    });
            },

            async saveEditing() {
                if (this.selectedItem) {
                    let formRef = null;
                    let endpoint = '';

                    if (this.isEmployeesTable) {
                        formRef = this.$refs.employeeForm;
                        endpoint = `https://localhost:7129/api/Employees/`;
                    } else if (this.isProjectsTable) {
                        formRef = this.$refs.projectForm;
                        endpoint = `https://localhost:7129/api/Projects/`;
                    }
                    const { valid } = await formRef.validate();
                    var dbRequest = axios.post;
                    if (this.selectedItem.id ?? 0 > 0) dbRequest = axios.put;

                    console.log(dbRequest)
                    if (valid) {
                        this.selectedItem = this.excludeNullOrEmptyProperties(this.selectedItem);
                        dbRequest(endpoint, this.selectedItem)
                            .then(response => {
                                this.snackbarText = `Item succesfully updated:\n${JSON.stringify(this.selectedItem)}`;
                                this.snackbarColor = 'blue';
                                this.snackbar = true;
                                this.showEditingDialog = false;
                                this.showCloseCheck = false;
                                this.selectedItem = null;

                                if (this.isEmployeesTable) {
                                    this.loadEmployees();
                                } else if (this.isProjectsTable) {
                                    this.loadProjects();
                                }
                            })
                            .catch(error => {
                                this.snackbarText = `Something went wrong on updating item...\n${error}`;
                                this.snackbarColor = '#ff3636';
                                this.snackbar = true;
                            });
                    }
                }
            },

            deleteEditing() {
                if (this.selectedItem) {
                    let endpoint = '';
                    if (this.isEmployeesTable) {
                        endpoint = `https://localhost:7129/api/Employees/${this.selectedItem.id}`;
                    } else if (this.isProjectsTable) {
                        endpoint = `https://localhost:7129/api/Projects/${this.selectedItem.id}`;
                    }

                    axios.delete(endpoint)
                        .then(response => {
                            this.snackbarText = `Item succesfully deleted:\n${this.selectedItem}`;
                            this.snackbarColor = 'blue';
                            this.snackbar = true;
                            this.showEditingDialog = false;
                            this.showCloseCheck = false;
                            this.selectedItem = null;

                            if (this.isEmployeesTable) {
                                this.loadEmployees();
                            } else if (this.isProjectsTable) {
                                this.loadProjects();
                            }
                        })
                        .catch(error => {
                            this.snackbarText = `Something went wrong on deleting item...\n${error}`;
                            this.snackbarColor = '#ff3636';
                            this.snackbar = true;
                        });
                }
            },

            getFullName(employee) {
                if (employee) {
                    var initials = '';
                    if (employee.surname) {
                        initials = `${employee.surname} ${employee.name.charAt(0)}.`;
                        if (employee.patronymic) initials += ` ${employee.patronymic.charAt(0)}.`;
                    } else {
                        initials = `${employee.name}`;
                        if (employee.patronymic) initials += ` ${employee.patronymic}`;
                    }
                    return initials;
                }
                return '';
            },

            handlePositionChange() {
                const newValue = this.selectedItem.position;
                const selectedExistingPosition = this.allPositions.find(position => position.name === newValue);

                if (!selectedExistingPosition) {
                    var newUserPosition = { id: 0, name: newValue };
                    this.allPositions.push(newUserPosition);
                    this.selectedItem.position = newUserPosition;
                }
            },

            changePosition() {
                this.showPositionDialog = true;
            },

            async updatePosition() {
                if (this.selectedItem.position) {

                    const { valid } = await this.$refs.positionForm.validate();

                    if (valid) {
                        axios.put('https://localhost:7129/api/Positions', this.selectedItem.position)
                            .then(response => {
                                this.showPositionDialog = false;
                                this.loadPositions();
                                this.selectedItem.position = response.data;
                                this.snackbarText = 'Position succesfully updated';
                                this.snackbarColor = 'blue';
                                this.snackbar = true;
                            })
                            .catch(error => {
                                this.snackbarText = `Something went wrong on updating position...\n${error}`;
                                this.snackbarColor = '#ff3636';
                                this.snackbar = true;
                            });
                    }
                }
            },

            async deletePosition() {
                if (this.selectedItem.position) {
                    axios.delete(`https://localhost:7129/api/Positions/${this.selectedItem.position.id}`)
                        .then(response => {
                            this.selectedItem.id = 0;
                            this.showPositionDialog = false;
                            this.loadPositions();
                            this.selectedItem.position = null;
                            this.snackbarText = 'Position succesfully deleted';
                            this.snackbarColor = 'blue';
                            this.snackbar = true;
                        })
                        .catch(error => {
                            this.snackbarText = `Something went wrong on deleting position...\n${error}`;
                            this.snackbarColor = '#ff3636';
                            this.snackbar = true;
                        });
                }
            },

            excludeNullOrEmptyProperties(obj) {
                return Object.keys(obj).reduce((acc, key) => {
                    if (obj[key] !== null && obj[key] !== '') {
                        acc[key] = obj[key];
                    }
                    return acc;
                }, {});
            },

            openEditingAgain() {
                this.showEditingDialog = true;
            }
        }
    }
</script>

<style scoped>
    tr.v-data-table__selected {
        background: #7d92f5 !important;
    }

    .v-data-table {
        margin-bottom: 20px;
    }

    .v-btn {
        margin: 0 8px 0 8px
    }

    .v-input__details {
        visibility: hidden;
    }
</style>