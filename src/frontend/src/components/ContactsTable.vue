<template>
  <DataTable
    :value="contacts"
    v-model:filters="filters"
    tableStyle="min-width: 50rem"
    filterDisplay="row"
    v-model:selection="selectedContacts"
  >
    <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
    <Column field="id" sortable header="Id"></Column>
    <Column filterField="name" field="name" sortable header="Name">
      <template #filter="{ filterModel, filterCallback }">
        <InputText
          type="text"
          v-model="filterModel.value"
          @input="filterCallback()"
          class="p-column-filter"
        />
      </template>
    </Column>
    <Column field="surname" sortable filter header="Surname">
      <template #filter="{ filterModel, filterCallback }">
        <InputText
          type="text"
          v-model="filterModel.value"
          @input="filterCallback()"
          class="p-column-filter"
        />
      </template>
    </Column>
    <Column field="email" sortable filter header="E-Mail">
      <template #filter="{ filterModel, filterCallback }">
        <InputText
          type="text"
          v-model="filterModel.value"
          @input="filterCallback()"
          class="p-column-filter"
        />
      </template>
    </Column>
    <Column field="cellPhoneNumber" sortable filter header="Cell Phone Number">
      <template #filter="{ filterModel, filterCallback }">
        <InputText
          type="text"
          v-model="filterModel.value"
          @input="filterCallback()"
          class="p-column-filter"
        />
      </template>
    </Column>
    <Column field="homePhoneNumber" sortable header="Home Phone Number">
      <template #filter="{ filterModel, filterCallback }">
        <InputText
          type="text"
          v-model="filterModel.value"
          @input="filterCallback()"
          class="p-column-filter"
        />
      </template>
    </Column>
  </DataTable>
</template>

<script setup lang="ts">
import { Contact } from "@/models/contact";
import InputText from "primevue/inputtext";
import DataTable, { DataTableFilterMeta, DataTableStateEvent } from "primevue/datatable";
import { FilterMatchMode } from "primevue/api";
import Column from "primevue/column";
import { ref, watch } from "vue";

const props = defineProps({
  contacts: Array<Contact>,
  selected: Array<Contact>,
});
const selectedContacts = ref<Contact[]>(props.selected ?? []);

const emit = defineEmits<{
  'update:selected': [rows: Contact[]]
}>();

const filters = ref<DataTableFilterMeta>({
  id: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  name: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  surname: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  email: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  cellPhoneNumber: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  homePhoneNumber: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
});


watch(selectedContacts, ()=>{
  console.log('contactsTable', selectedContacts);
  emit('update:selected', selectedContacts.value);
});

watch(props.selected ?? [], () => {
  console.log('selected changed!', props.selected);
  selectedContacts.value = props.selected ?? [];
});

function selectionChanged(event: any) {
  console.log(event);
  // selectedContacts.value.push(event);
  emit("update:selected", selectedContacts.value);
}
</script>

<style lang="scss" scoped></style>
