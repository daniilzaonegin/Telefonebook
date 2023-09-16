<template>
  <BaseLayout>
    <h2>Please enter your google credentials to login</h2>
    <form @submit="onSubmit">
      <div class="card flex flex-column gap-3">
        <div class="p-inputgroup align-content-start" style="width: 30rem">
          <span class="p-inputgroup-addon">
            <i class="pi pi-user"></i>
          </span>
          <InputText
            placeholder="Login"
            v-bind="login"
            :class="{ 'p-invalid': errors.login }"
          />
        </div>
        <small class="p-error">
          {{ errors.login }}
        </small>

        <div class="p-inputgroup" style="width: 30rem">
          <span class="p-inputgroup-addon">
            <i class="pi pi-lock"></i>
          </span>
          <InputText
            placeholder="Password"
            type="password"
            v-bind="password"
            :class="{ 'p-invalid': errors.password }"
          />
        </div>
        <small class="p-error">
          {{ errors.password }}
        </small>

        <div class="flex" style="width: 30rem">
          <Button type="submit" label="Login" style="margin-right: 0.5rem;" />
          <Button
            type="button"
            label="Register"
            icon="pi pi-user-edit"
            severity="secondary"
            @click="register"
          />
        </div>
      </div>
    </form>
  </BaseLayout>
</template>

<script lang="ts" setup>
import BaseLayout from "@/layouts/BaseLayout.vue";
import router from "@/router";
import { useUserDataStore } from "@/store/usersStore";
import Button from "primevue/button";
import InputText from "primevue/inputtext";
import { useToast } from "primevue/usetoast";
import { useForm } from "vee-validate";
import * as yup from "yup";

const userDataStore = useUserDataStore();
const toast = useToast();

const schema = yup.object({
  login: yup.string().required().label("Login"),
  password: yup.string().required().label("Password"),
});

const { defineComponentBinds, handleSubmit, setFieldValue, errors } = useForm({
  validationSchema: schema,
});

const login = defineComponentBinds("login");
const password = defineComponentBinds("password");

const onSubmit = handleSubmit(async (values: any) => {
  try {
    await userDataStore.loginUser({
      username: values.login,
      providerKey: values.password,
    });
    router.push("/");
  } catch (error) {
    toast.add({
      severity: "error",
      summary: "Error",
      detail: `Login failed. Error message: ${error}`,
      life: 3000,
      group: "msg",
    });
  }
});
const register = () => {
  router.push("/Register");
};
</script>

<style lang="scss" scoped></style>
