<template>
    <div v-if="loaded" style="position: sticky; top: 85%; height: 50px;">
        <v-btn size="x-large" color="blue" icon="mdi-plus" @click="addNewItem()"></v-btn>
    </div>
    <div class="main">
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

        <v-dialog v-model="showDialog" max-width="500px">
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
                                <v-col cols="12" sm="6">
                                    <v-combobox variant="outlined" 
                                                v-model="selectedItem.position" 
                                                :items="allPositions" 
                                                label="Position" 
                                                item-title="name" 
                                                :rules="[v => v => (v && v.length <= 100) || 'Maximum 100 characters']"
                                                @change="handlePositionChange"></v-combobox>
                                </v-col>
                            </v-row>
                        </v-form>
                        <v-form ref="projectForm" v-else-if="isProjectsTable">
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field variant="outlined" v-model="selectedItem.name" label="Name" maxlength="100" :rules="[v => !!v || 'Name is required', v => (v && v.length <= 100) || 'Maximum 100 characters']"></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field variant="outlined" v-model="selectedItem.customerCompany" label="Customer Company" maxlength="100" :rules="[v => !v || (v && v.length <= 100) || 'Maximum 100 characters']"></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field variant="outlined" v-model="selectedItem.executorCompany" label="Executor Company" maxlength="100"></v-text-field>
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
                                    <v-select variant="outlined" v-model="selectedItem.projectManager" :items="allEmployees" label="Project Manager" :item-title="getFullName"></v-select>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-select variant="outlined" v-model="selectedItem.employees" :items="allEmployees" label="Assigned employees" :item-title="getFullName" multiple></v-select>
                                </v-col>
                            </v-row>
                        </v-form>
                    </template>
                </v-card-text>
                <v-card-actions style="justify-content: space-between">
                    <v-btn size="large" variant="flat" color="green" @click="saveEditing">Save</v-btn>
                    <div v-if="selectedItem?.id">
                        <v-btn variant="flat" color="red" @click="deleteEditing">Delete</v-btn>
                    </div>
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
                showDialog: false,
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
                allPositions: []
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
            }
        },
        methods: {
            rowClick: function (item, row) {
                this.selectedItem = JSON.parse(JSON.stringify(row.item.columns));
                this.showDialog = true;
                console.log(this.selectedItem)
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
                this.showDialog = true;
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
                        console.error('Error loading employees:', error);
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
                        console.error('Error loading employees:', error);
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
                    }];

                axios.get('https://localhost:7129/api/Employees')
                    .then(response => {
                        this.allEmployees = response.data;
                        this.loaded = true;
                    })
                    .catch(error => {
                        console.error('Error loading employees:', error);
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
                        console.error('Error loading employees:', error);
                        this.loaded = true;
                    });
            },

            async saveEditing() {
                if (this.selectedItem) {
                    let formRef = null;
                    let endpoint = '';

                    if (this.isEmployeesTable) {
                        formRef = this.$refs.employeeForm;
                        endpoint = `https://localhost:7129/api/Employees/`;
                    } else if (this.projectForm) {
                        formRef = this.$refs.projectsForm;
                        endpoint = `https://localhost:7129/api/Projects/`;
                    }

                    const { valid } = await formRef.validate();


                    if (valid) {
                        console.log(this.selectedItem)
                        axios.put(endpoint, this.selectedItem)
                            .then(response => {
                                console.log('Item saved:', response.data);
                                this.showDialog = false;
                                this.selectedItem = null;

                                if (this.isEmployeesTable) {
                                    this.loadEmployees();
                                } else if (this.isProjectsTable) {
                                    this.loadProjects();
                                }
                            })
                            .catch(error => {
                                console.error('Error saving item:', error);
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
                            console.log('Item deleted:', response.data);
                            this.showDialog = false;
                            this.selectedItem = null;

                            if (this.isEmployeesTable) {
                                this.loadEmployees();
                            } else if (this.isProjectsTable) {
                                this.loadProjects();
                            }
                        })
                        .catch(error => {
                            console.error('Error deleting item:', error);
                        });
                }
            },

            getFullName(employee) {
                console.log(this.selectedItem)
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