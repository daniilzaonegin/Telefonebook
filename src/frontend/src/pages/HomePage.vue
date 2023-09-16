<template>
  <BaseLayout>
    <template #toolbar>
      <Button
        v-tooltip="'Refresh'"
        style="margin-right: 3px"
        icon="pi pi-refresh"
        size="small"
        @click="refresh"
      />
      <Button
        v-tooltip="'Add'"
        style="margin-right: 3px"
        icon="pi pi-plus"
        size="small"
        @click="add"
      />
      <Button
        v-tooltip="'Delete'"
        style="margin-right: 3px"
        icon="pi pi-times"
        size="small"
        severity="danger"
        @click="remove"
      />
      <Button
        v-tooltip="'Edit'"
        :disabled="!editButtonEnabled"
        icon="pi pi-eraser"
        size="small"
        @click="edit"
      />
      <ProgressSpinner
        v-if="loading"
        style="width: 28px; height: 28px"
        strokeWidth="5"
        animationDuration=".5s"
        aria-label="loading"
      />

      <div v-if="errorMessage" style="color: red">{{ errorMessage }}</div>
    </template>
    <template #default>
      <ContactsTable
        :key="contactsTableKey"
        :contacts="contacts"
        v-model:selected="selectedContacts"
        />
    </template>
  </BaseLayout>
</template>

<script setup lang="ts">
import { Contact } from "@/models/contact";
import BaseLayout from "@/layouts/BaseLayout.vue";
import { useContactsStore } from "@/store/contactsStore";
import ContactsTable from "@/components/ContactsTable.vue";
import { computed, ref } from "vue";
import Button from "primevue/button";
import router from "@/router";
import ProgressSpinner from "primevue/progressspinner";
import { useToast } from "primevue/usetoast";

const contactsStore = useContactsStore();
const contacts = ref<Contact[]>([]);
const errorMessage = ref<string>();
const loading = ref<boolean>(false);
const selectedContacts = ref<Contact[]>([] as Contact[]);
const toast = useToast();
const contactsTableKey = ref(0);
const editButtonEnabled = computed<boolean>(() => selectedContacts.value.length == 1);

refresh();

function refresh() {
  loading.value = true;
  contactsStore
    .fetchContacts()
    .then(() => {
      contacts.value = [...contactsStore.allContacts];
      errorMessage.value = "";
      loading.value = false;
    })
    .catch(() => {
      errorMessage.value = contactsStore.errorMessage;
      loading.value = false;
    });
}

function add() {
  //open a form page with add fields
  router.push("/add");
}

async function edit() {
  if (!selectedContacts.value) {
    toast.add({
      severity: "error",
      summary: "Error",
      detail: "Nothing is selected",
      life: 3000,
      group: "msg",
    });
    return;
  }
  router.push(`/edit/${selectedContacts.value[0].id}`);
}

async function remove() {
  if (!selectedContacts.value) {
    toast.add({
      severity: "error",
      summary: "Error",
      detail: "Nothing is selected",
      life: 3000,
      group: "msg",
    });
    return;
  }

  try {
    await contactsStore.removeContacts(selectedContacts.value);
    selectedContacts.value = [];
    refresh();
    reloadContactsTable();
    toast.add({
      severity: "success",
      summary: "Contacts were deleted",
      detail: "Contacts were deleted",
      life: 3000,
      group: "msg",
    });
  } catch (error) {
    console.log(error);
    toast.add({
      severity: "error",
      summary: "Deletion Error",
      detail: error,
      life: 3000,
      group: "msg",
    });
  }
}

function reloadContactsTable() {
  contactsTableKey.value += 1;
}
</script>

<style lang="scss" scoped></style>
